using LinkDev.Ticketing.Application.Interfaces;
using LinkDev.Ticketing.Core.Models;
using LinkDev.Ticketing.Domain.Entities;
using LinkDev.Ticketing.Infrastructure.Data;
using LinkDev.Ticketing.Infrastructure.Helpers;
using Microsoft.Data.SqlClient;
using System.Data;

namespace LinkDev.Ticketing.Infrastructure.Repositories
{
    public class TicketRepository : Repository<Ticket>, ITicketRepository
    {
        private readonly DBHelper _dBHelper;
        public TicketRepository(TicketingContext dbContext, DBHelper dBHelper) : base(dbContext)
        {
            _dBHelper = dBHelper;
        }

        public IEnumerable<TicketView> GetTickets(TicketRequestDTO requestDTO, out int totalCount)
        {
            DataSet result =_dBHelper.GetDataSetFromSP(null, "GetTickets",
                new SqlParameter("@Lang", requestDTO.Culture?.ToLower() == "en-us" ? (short)1 : (short)2),
                new SqlParameter("@PageNumber", requestDTO.PageNumber), 
                new SqlParameter("@PageSize", requestDTO.PageSize), 
                new SqlParameter("@SearchValue", requestDTO.SearchValue ?? string.Empty), 
                new SqlParameter("@SortBy", requestDTO.SortBy ?? string.Empty), 
                new SqlParameter("@SortDir", requestDTO.SortDirection ?? string.Empty));

            totalCount = Convert.ToInt32(result.Tables[0].Rows[0]["CountRows"]);

            List<TicketView> tickets = new List<TicketView>();
            foreach (DataRow row in result.Tables[1].Rows)
            {
                tickets.Add(new TicketView
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Title = row["Title"].ToString(),
                    Description = row["Description"].ToString(),
                    //TypeId = Convert.ToInt32(row["TypeId"]),
                    //CategoryId = Convert.ToInt32(row["CategoryId"]),
                    //SubCategoryId = Convert.ToInt32(row["SubCategoryId"]),
                    //PriorityId = Convert.ToInt32(row["PriorityId"]),
                    //StatusId = Convert.ToInt32(row["StatusId"]),
                    CreatedBy = row["CreatedBy"].ToString(),
                    CreatedAt = Convert.ToDateTime(row["CreatedAt"]),
                    ModifiedBy = row["ModifiedBy"].ToString(),
                    LastModifiedAt = row["LastModifiedAt"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["LastModifiedAt"]),
                    Status = row["Status"].ToString(),
                    Priority = row["Priority"].ToString(),
                    TicketType = row["TicketType"].ToString(),
                    TicketCategory = row["TicketCategory"].ToString()
                    //TicketSubCategory = row["TicketSubCategory"].ToString()
                });
            }
            return tickets;
            //return _dBContext.Database.SqlQueryRaw<TicketView>("execute GetTickets @Lang", new SqlParameter("@Lang" , LangCode));
        }
    }
}
