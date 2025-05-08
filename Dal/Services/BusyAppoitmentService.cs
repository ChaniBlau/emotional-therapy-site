using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dal.Api;
using Dal.Models;
using Microsoft.EntityFrameworkCore;

namespace Dal.Services
{
    public class BusyAppoitmentService : IBusyAppointment
    {
        private readonly DatabaseManager _databaseManager;
        public BusyAppoitmentService(DatabaseManager databaseManager)
        {
            _databaseManager = databaseManager;
        }
        public async Task<bool> CreateAsync(BusyAppointment entity)
        {
            await _databaseManager.BusyAppointments.AddAsync(entity);
            var result = await _databaseManager.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            await _databaseManager.BusyAppointments.Where(x => x.Code == int.Parse(id)).ExecuteDeleteAsync();
            var result = await _databaseManager.SaveChangesAsync();
            return result > 0;
        }

        public async Task<IEnumerable<BusyAppointment>> ReadAllAsync() => await _databaseManager.BusyAppointments.ToListAsync();


        public async Task<BusyAppointment> ReadByIdAsync(string id) => await _databaseManager.BusyAppointments.FindAsync(int.Parse(id)) ?? throw new ArgumentNullException("Busy appointment not found");


        public async Task<bool> UpdateAsync(BusyAppointment entity)
        {
            await _databaseManager.BusyAppointments.Where(x => x.Code == entity.Code).ExecuteUpdateAsync(setters => setters
             .SetProperty(x => x.Code, entity.Code)
             .SetProperty(x => x.Date, entity.Date)
             .SetProperty(x => x.Time, entity.Time)
             .SetProperty(x => x.TherapistId, entity.TherapistId)
             .SetProperty(x => x.ClientId, entity.ClientId));
             
            var result = await _databaseManager.SaveChangesAsync();
            return result > 0;
        }
    }
}
