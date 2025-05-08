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
    public class TherapistService : ITherapist
    {
        private readonly DatabaseManager _databaseManager;
        public TherapistService(DatabaseManager databaseManager)
        {
            _databaseManager = databaseManager;
        }
        public async Task<bool> CreateAsync(Therapist entity)
        {
            await _databaseManager.Therapists.AddAsync(entity);
            var result = await _databaseManager.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("Id cannot be null");
            }
            await _databaseManager.Therapists.Where(x => x.Id == id).ExecuteDeleteAsync();
            var result = await _databaseManager.SaveChangesAsync();
            return result > 0;
        }

        public async Task<IEnumerable<Therapist>> ReadAllAsync() => await _databaseManager.Therapists.ToListAsync();


        public async Task<Therapist> ReadByIdAsync(string id) => await _databaseManager.Therapists.FindAsync(id) ?? throw new ArgumentNullException("Therapist not found");

        public async Task<bool> UpdateAsync(Therapist entity)
        {
            await _databaseManager.Therapists.Where(x=>x.Id== entity.Id).ExecuteUpdateAsync(setters => setters
            .SetProperty(x => x.FirstName, entity.FirstName)
            .SetProperty(x => x.LastName, entity.LastName)
            .SetProperty(x => x.Specialization, entity.Specialization)
            .SetProperty(x => x.City, entity.City)
            .SetProperty(x => x.StartingWorkYear, entity.StartingWorkYear)
            .SetProperty(x => x.SalaryPerMonth, entity.SalaryPerMonth)
            .SetProperty(x => x.Email, entity.Email)
            .SetProperty(x => x.PhoneNumber, entity.PhoneNumber));
            var result = await _databaseManager.SaveChangesAsync();
            return result > 0;


        }
    }
}
