using LinkDev.Ticketing.Application.Dtos;
using LinkDev.Ticketing.Application.Interfaces;
using LinkDev.Ticketing.Core.Models;
using LinkDev.Ticketing.Domain.Entities;
using LinkDev.Ticketing.Domain.Enums;
using LinkDev.Ticketing.Infrastructure.Data;
using LinkDev.UserManagent.Application.Interfaces;
using LinkDev.UserManagent.Domain.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text;

namespace LinkDev.Ticketing.Infrastructure.Repositories
{
    public class TicketRepository : Repository<Ticket>, ITicketRepository
    {
        private readonly IUserManager _userManager;
        private readonly ILookupRepository _lookupRepository;
        private readonly Logging.Application.Interfaces.ILogger _logger;

        public TicketRepository(TicketingContext dbContext
            , IUserManager userManager
            , ILookupRepository lookupRepository
            , Logging.Application.Interfaces.ILogger logger) : base(dbContext)
        {
            _userManager = userManager;
            _lookupRepository = lookupRepository;
            _logger = logger;
        }

        public IEnumerable<TicketView> GetTickets(TicketRequestDTO requestDTO, string userId, Guid correlationId, out int totalCount)
        {
            StringBuilder query = new StringBuilder();
            StringBuilder filter = new StringBuilder(" WHERE 1=1");

            List<SqlParameter> sqlParameters = new List<SqlParameter>(){
                new SqlParameter("@Lang", requestDTO.Culture?.ToLower() == "en-us" ? (short)1 : (short)2)
            };

            if (_userManager.IsInRole(UserRoles.Client))
            {
                filter.Append(" AND CreatedBy = @UserId");
                sqlParameters.Add(new SqlParameter("@UserId", userId));
            }

            if (!string.IsNullOrWhiteSpace(requestDTO.SearchValue))
            {
                requestDTO.SearchValue = $"%{requestDTO.SearchValue}%";
                filter.Append(" AND Title like @SearchValue");
                sqlParameters.Add(new SqlParameter("@SearchValue", requestDTO.SearchValue));
            }

            if (requestDTO.TicketType > 0)
            {
                filter.Append(" AND Ticket.Type=@Type");
                sqlParameters.Add(new SqlParameter("@Type", requestDTO.TicketType));
            }
            if (requestDTO.TicketStatus > 0)
            {
                filter.Append(" AND Ticket.Status=@Status");
                sqlParameters.Add(new SqlParameter("@Status", requestDTO.TicketStatus));
            }
            if (requestDTO.TicketCategory > 0)
            {
                filter.Append(" AND Ticket.Category=@Category");
                sqlParameters.Add(new SqlParameter("@Category", requestDTO.TicketCategory));
            }
            if (requestDTO.TicketPriority > 0)
            {
                filter.Append(" AND Ticket.Priority=@Priority");
                sqlParameters.Add(new SqlParameter("@Priority", requestDTO.TicketPriority));
            }

            string queryFilter = filter.ToString();
            if (queryFilter == " WHERE 1=1") //Empty Filter
            {
                queryFilter = "";
            }
            else
            {
                queryFilter = queryFilter.Replace("1=1 AND", "");
            }

            if (!new string[] { "Id", "Title", "CreatedAt" }.Contains(requestDTO.SortBy))
            {
                requestDTO.SortBy = "Ticket.[Id]";
            }
            if (string.IsNullOrWhiteSpace(requestDTO.SortDirection)
                || (requestDTO.SortDirection.ToLower() != "asc" && requestDTO.SortDirection.ToLower() != "desc"))
            {
                requestDTO.SortDirection = "asc";
            }

            string getCountQuery = @$"SELECT count(*) [Value]
            FROM Ticket
            INNER JOIN TicketStatusLookup 
                ON TicketStatusLookup.Id = [Ticket].Status 
                AND TicketStatusLookup.LangId = @Lang {filter}";

            query.AppendFormat(@"
            SELECT Ticket.[Id],
                   [Title],
                   [Description],
                   [CreatedBy],
                   [CreatedAt],
                   [ModifiedBy],
                   [LastModifiedAt],
                   TicketStatusLookup.Name AS [Status],
                   TicketPriorityLookup.Name AS [Priority],
                   TicketTypeLookup.Name AS [TicketType],
                   TicketCategoryLookup.Name AS [TicketCategory]
            FROM Ticket
            INNER JOIN TicketStatusLookup 
                ON TicketStatusLookup.Id = [Ticket].Status 
                AND TicketStatusLookup.LangId = @Lang
            INNER JOIN TicketPriorityLookup
                ON TicketPriorityLookup.Id = [Ticket].Priority 
                AND TicketPriorityLookup.LangId = @Lang
            INNER JOIN TicketTypeLookup 
                ON TicketTypeLookup.Id = [Ticket].Type 
                AND TicketTypeLookup.LangId = @Lang
            INNER JOIN TicketCategoryLookup 
                ON TicketCategoryLookup.Id = [Ticket].Category 
                AND TicketCategoryLookup.LangId = @Lang
            {0}
            ORDER BY {1} {2}
            OFFSET @Start ROWS
            FETCH NEXT @PageSize ROWS ONLY", queryFilter, requestDTO.SortBy, requestDTO.SortDirection);

            totalCount = _dBContext.Set<ScalarInt>()
            .FromSqlRaw(getCountQuery, sqlParameters.ToArray())
            .Select(x => x.Value)
            .First();

            sqlParameters.Add(new SqlParameter("@Start", (requestDTO.PageNumber - 1) * requestDTO.PageSize));
            sqlParameters.Add(new SqlParameter("@PageSize", requestDTO.PageSize));
            sqlParameters.Add(new SqlParameter("@SortBy", requestDTO.SortBy));
            sqlParameters.Add(new SqlParameter("@SortDir", requestDTO.SortDirection));

            List<TicketView> tickets = _dBContext.Database.SqlQueryRaw<TicketView>(query.ToString(),sqlParameters.ToArray()).ToList();

            return tickets;
        }

        public List<TicketStatisticsDTO>? GetTicketStatistics(string culture, string userId, Guid correlationId)
        {
            try
            {
                var lookupStatus = _lookupRepository.GetLookup<BaseLookup>(LookupType.TicketStatus, culture);
                var lookupPriority = _lookupRepository.GetLookup<BaseLookup>(LookupType.TicketPriority, culture);

                if(lookupStatus == null)
                {
                    _logger.LogInformation("lookupStatus is null", "TicketingRepository", "GetTicketStatistics", correlationId);
                    return null;
                }
                if (lookupPriority == null) 
                {
                    _logger.LogInformation("lookupPriority is null", "TicketingRepository", "GetTicketStatistics", correlationId);
                    return null;
                }

                List<TicketStatisticsDTO> statisticsDTOs = new List<TicketStatisticsDTO>();

                var ticketStatusGroup = _dBContext.Tickets.GroupBy(x => x.Status).AsEnumerable();

                var ticketByStatus = new TicketStatisticsDTO();
                ticketByStatus.KeyIndicators = ticketStatusGroup.Select(x => new KeyIndicator()
                {
                    Key = x.Key.ToString(),
                    Value = x.Count().ToString(),
                    Name = lookupStatus.FirstOrDefault(i => i.Id == x.Key)?.Name
                }).ToList();


                var ticketPriorityGroup = _dBContext.Tickets.GroupBy(x => x.Priority).AsEnumerable();

                var ticketByPriority = new TicketStatisticsDTO();
                ticketByPriority.KeyIndicators = ticketPriorityGroup.Select(x => new KeyIndicator()
                {
                    Key = x.Key.ToString(),
                    Value = x.Count().ToString(),
                    Name = lookupPriority.FirstOrDefault(i => i.Id == x.Key)?.Name
                }).ToList();

                statisticsDTOs.Add(ticketByStatus);
                statisticsDTOs.Add(ticketByPriority);

                return statisticsDTOs;
            }
            catch (Exception exp)
            {
                _logger.LogError(exp, "Exception in GetTicketStatistics", "TicketingRepository", "GetTicketStatistics", correlationId);

                return null;
            }
        }
    }
}
