using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using ELabel.Web.DataAccess;
using ELabel.Web.Models;

namespace ELabel.Web.Repositories
{
    public class RecoveryIntervalsRepository : IRepository<RecoveryIntervalsDTO>
    {
        private elabelEntities context;
        private DbSet<RecoveryInterval> dbSet;

        public RecoveryIntervalsRepository()
        {
            context = new elabelEntities();
            dbSet = context.Set<RecoveryInterval>();
        }

        public void Add(RecoveryIntervalsDTO entity)
        {
            var item = new RecoveryInterval
            {
                HOLEID = entity.HOLEID,
                PROJECTCODE = entity.PROJECTCODE,
                GEOLFROM = entity.GEOLFROM,
                GEOLTO = entity.GEOLTO,
                PRIORITY = entity.PRIORITY,
                Recovery_m = entity.Recovery_m,
                RecovGeo = entity.RecovGeo,
                RecovGeoDate = entity.RecovGeoDate,
                CoreLength = entity.CoreLength,
                RecoveryRatio = entity.RecoveryRatio,
                ID = entity.ID
            };

            dbSet.Add(item);
        }

        public void AddAll(IEnumerable<RecoveryIntervalsDTO> entityList)
        {
            var items = entityList.Select(x => new RecoveryInterval
            {
                HOLEID = x.HOLEID,
                PROJECTCODE = x.PROJECTCODE,
                GEOLFROM = x.GEOLFROM,
                GEOLTO = x.GEOLTO,
                PRIORITY = x.PRIORITY,
                Recovery_m = x.Recovery_m,
                RecovGeo = x.RecovGeo,
                RecovGeoDate = x.RecovGeoDate,
                CoreLength = x.CoreLength,
                RecoveryRatio = x.RecoveryRatio,
                ID = x.ID
            });

            dbSet.AddRange(items);
        }

        public void Delete(RecoveryIntervalsDTO entity)
        {
            var item = new RecoveryInterval
            {
                HOLEID = entity.HOLEID,
                PROJECTCODE = entity.PROJECTCODE,
                GEOLFROM = entity.GEOLFROM,
                GEOLTO = entity.GEOLTO,
                PRIORITY = entity.PRIORITY,
                Recovery_m = entity.Recovery_m,
                RecovGeo = entity.RecovGeo,
                RecovGeoDate = entity.RecovGeoDate,
                CoreLength = entity.CoreLength,
                RecoveryRatio = entity.RecoveryRatio,
                ID = entity.ID
            };

            dbSet.Remove(item);
        }

        public void Put(RecoveryIntervalsDTO entity)
        {
            var item = new RecoveryInterval
            {
                HOLEID = entity.HOLEID,
                PROJECTCODE = entity.PROJECTCODE,
                GEOLFROM = entity.GEOLFROM,
                GEOLTO = entity.GEOLTO,
                PRIORITY = entity.PRIORITY,
                Recovery_m = entity.Recovery_m,
                RecovGeo = entity.RecovGeo,
                RecovGeoDate = entity.RecovGeoDate,
                CoreLength = entity.CoreLength,
                RecoveryRatio = entity.RecoveryRatio,
                ID = entity.ID
            };

            context.Entry(item).State = EntityState.Modified;
        }

        public void Update(RecoveryIntervalsDTO entity)
        {
            var item = new RecoveryInterval
            {
                HOLEID = entity.HOLEID,
                PROJECTCODE = entity.PROJECTCODE,
                GEOLFROM = entity.GEOLFROM,
                GEOLTO = entity.GEOLTO,
                PRIORITY = entity.PRIORITY,
                Recovery_m = entity.Recovery_m,
                RecovGeo = entity.RecovGeo,
                RecovGeoDate = entity.RecovGeoDate,
                CoreLength = entity.CoreLength,
                RecoveryRatio = entity.RecoveryRatio,
                ID = entity.ID
            };

            dbSet.Attach(item);
            context.Entry(item).State = EntityState.Modified;
        }

        public void UpdateAll(IList<RecoveryIntervalsDTO> entity)
        {
            foreach (var item in entity)
            {
                var interval = new RecoveryInterval
                {
                    HOLEID = item.HOLEID,
                    PROJECTCODE = item.PROJECTCODE,
                    GEOLFROM = item.GEOLFROM,
                    GEOLTO = item.GEOLTO,
                    PRIORITY = item.PRIORITY,
                    Recovery_m = item.Recovery_m,
                    RecovGeo = item.RecovGeo,
                    RecovGeoDate = item.RecovGeoDate,
                    CoreLength = item.CoreLength,
                    RecoveryRatio = item.RecoveryRatio,
                    ID = item.ID
                };

                dbSet.Attach(interval);
                context.Entry(interval).State = EntityState.Modified;
            }
        }

        public IEnumerable<RecoveryIntervalsDTO> GetAll()
        {
            return dbSet.Select(x => new RecoveryIntervalsDTO
            {
                HOLEID = x.HOLEID,
                PROJECTCODE = x.PROJECTCODE,
                GEOLFROM = x.GEOLFROM,
                GEOLTO = x.GEOLTO,
                PRIORITY = x.PRIORITY,
                Recovery_m = x.Recovery_m,
                RecovGeo = x.RecovGeo,
                RecovGeoDate = x.RecovGeoDate,
                CoreLength = x.CoreLength,
                RecoveryRatio = x.RecoveryRatio,
                ID = x.ID,
                LL_WGS84_Calc_X = x.Drillhole.LL_WGS84_Calc_X,
                LL_WGS84_Calc_Y = x.Drillhole.LL_WGS84_Calc_Y
            });
        }

        public RecoveryIntervalsDTO Find(string id)
        {
            var item = dbSet.Find(id);

            return new RecoveryIntervalsDTO
            {
                HOLEID = item.HOLEID,
                PROJECTCODE = item.PROJECTCODE,
                GEOLFROM = item.GEOLFROM,
                GEOLTO = item.GEOLTO,
                PRIORITY = item.PRIORITY,
                Recovery_m = item.Recovery_m,
                RecovGeo = item.RecovGeo,
                RecovGeoDate = item.RecovGeoDate,
                CoreLength = item.CoreLength,
                RecoveryRatio = item.RecoveryRatio,
                ID = item.ID,
                LL_WGS84_Calc_X = item.Drillhole.LL_WGS84_Calc_X,
                LL_WGS84_Calc_Y = item.Drillhole.LL_WGS84_Calc_Y
            };
        }

        public IEnumerable<RecoveryIntervalsDTO> FindAll(Expression<Func<RecoveryIntervalsDTO, bool>> predicate)
        {
            return null;
        }

        public int Count(Expression<Func<RecoveryIntervalsDTO, bool>> predicate)
        {
            return 0;
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}