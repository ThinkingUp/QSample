using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Raven.Client;
using Raven.Client.Document;

namespace ELabel.QC.Repository
{
    public class AlertSpecificationRepository : IRepository<AlertDefinition>
    {

        private IDocumentStore docStore = new DocumentStore
        {
            //Url = "http://localhost:8080",
            //Url = "http://www.soo-net.co:8080",
            Url = "http://YUURA:8080",
            DefaultDatabase = "Elabel",
        };

        public AlertSpecificationRepository()
        {
            docStore.Initialize();
        }
        public long Add(AlertDefinition newDefinition)
        {
            long result;

            using (var session = docStore.OpenSession())
            {
                session.Store(newDefinition);
                session.SaveChanges();

                result = newDefinition.Id;

            }

            return result;
        }

        public bool AddAll(IList<AlertDefinition> newDefinitions)
        {
            var result = false;

            using (var session = docStore.OpenSession())
            {
                try
                {
                    foreach (var alertDefinition in newDefinitions)
                    {
                        session.Store(alertDefinition);
                    }
                    session.SaveChanges();
                    result = true;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            return result;
        }

        public void Delete(AlertDefinition targetDefinition)
        {
            using (var session = docStore.OpenSession())
            {

                session.Delete(session.Load<AlertDefinition>(targetDefinition.Id));
                session.SaveChanges();
            }
        }

        public void Update(AlertDefinition updatedDefinition)
        {
            using (var session = docStore.OpenSession())
            {
                var targetDefinition = GetById(updatedDefinition.Id);

                if (targetDefinition != null) targetDefinition = MapUpdates(updatedDefinition, targetDefinition);

                session.Store(targetDefinition);
                session.SaveChanges();
            }
        }

        public void UpdateAll(IList<AlertDefinition> updatedDefinitions)
        {
            using (var session = docStore.OpenSession())
            {
                try
                {
                    foreach (var updatedDefinition in updatedDefinitions)
                    {
                        var targetDefinition = GetById(updatedDefinition.Id);

                        if (targetDefinition != null)
                            targetDefinition = MapUpdates(updatedDefinition, targetDefinition);

                        session.Store(targetDefinition);
                    }
                }
                catch (Exception ex)
                {
                    //todo log?
                    throw;
                }
                session.SaveChanges();
            }
        }

        public AlertDefinition GetById(long id)
        {
            AlertDefinition result = null;
            try
            {
                using (var session = docStore.OpenSession())
                {

                    result = session.Load<AlertDefinition>(id);
                }

            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        public IList<AlertDefinition> FindAll()
        {
            return FindAll(alert => alert.Active).ToList(); ;
        }

        public IList<AlertDefinition> FindAll(Expression<Func<AlertDefinition, bool>> predicate)
        {
            var result = new List<AlertDefinition>();

            using (var session = docStore.OpenSession())
            {
                try
                {
                    result = session.Query<AlertDefinition>().Where(predicate).ToList();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            return result;
        }

        public int Count(Expression<Func<AlertDefinition, bool>> predicate)
        {
            return FindAll(predicate).Count;
        }

        public void RefreshSession()
        {
            throw new NotImplementedException();
        }

        private static AlertDefinition MapUpdates(AlertDefinition updatedDefinition, AlertDefinition targeDefinition)
        {
            targeDefinition.Active = updatedDefinition.Active;
            //do the rest
            targeDefinition.LastUpdated = DateTime.Now;

            return targeDefinition;
        }
    }
}
