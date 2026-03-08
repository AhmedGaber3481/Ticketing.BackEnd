using LinkDev.Ticketing.Application.Interfaces;
using LinkDev.Ticketing.Core.Models;
using LinkDev.Ticketing.Domain.Entities;
using LinkDev.Ticketing.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text;

namespace LinkDev.Ticketing.Infrastructure.Repositories
{
    public class TicketRepository : Repository<Ticket>, ITicketRepository
    {
        public TicketRepository(TicketingContext dbContext) : base(dbContext)
        {
        }

        public IEnumerable<TicketView> GetTickets(TicketRequestDTO requestDTO, out int totalCount)
        {
            StringBuilder query = new StringBuilder();
            //StringBuilder getCountQuery = new StringBuilder();
            StringBuilder filter = new StringBuilder(" WHERE 1=1");

            List<SqlParameter> sqlParameters = new List<SqlParameter>(){
                new SqlParameter("@Lang", requestDTO.Culture?.ToLower() == "en-us" ? (short)1 : (short)2)
            };

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

            //getCountQuery.Append(@"
            //SELECT count(*) [Value]
            //FROM Ticket
            //INNER JOIN TicketStatusLookup 
            //    ON TicketStatusLookup.Id = [Ticket].Status 
            //    AND TicketStatusLookup.LangId = @Lang")
            //.Append(filter.ToString());

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
    }
}
