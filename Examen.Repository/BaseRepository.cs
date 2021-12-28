using Examen.Utilities.Ado;
using Examen.Utilities.General;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Examen.Repository
{
    public class BaseRepository : IDisposable
    {
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #region Metodo que devuelve un Datatable
        public virtual async Task<DataSet> DataSet(string qry, params object[] args)
        {
            try
            {
                using (var Ado = new SQLServer(GeneralModel.ConnectionString))
                {
                    var dt = await Ado.ExecDataSetProc(qry, args);
                    return dt;
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Metodo general que devuelve una lista
        public virtual async Task<List<T>> Find<T>(string qry, params object[] args) where T : class
        {
            try
            {
                using (var Ado = new SQLServer(GeneralModel.ConnectionString))
                {
                    List<T> list = new List<T>();
                    T obj = default(T);
                    var Dr = await Ado.ExecDataReaderProcTrans(qry, args);
                    if (!Dr.HasRows) return null;
                    while (await Dr.ReadAsync())
                    {

                        obj = Activator.CreateInstance<T>();

                        for (var i = 0; i < Dr.FieldCount; i++)
                        {
                            foreach (PropertyInfo prop in obj.GetType().GetProperties())
                            {
                                if (Dr.GetName(i).ToUpper() == prop.Name.ToUpper())
                                {
                                    if (!object.Equals(Dr[prop.Name], DBNull.Value))
                                    {
                                        prop.SetValue(obj, Convert.ChangeType(Dr[prop.Name], prop.PropertyType), null);
                                        break;
                                    }
                                }

                            }
                        }
                        list.Add(obj);
                    }
                    Dr.Close();
                    return list;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region Metodo general que devuelve un objecto (clase)
        public virtual async Task<T> FindOne<T>(string qry, params object[] args) where T : class
        {
            try
            {
                using (var Ado = new SQLServer(GeneralModel.ConnectionString))
                {
                    T obj = default(T);
                    var Dr = await Ado.ExecDataReaderProcTrans(qry, args);
                    if (!Dr.HasRows) return null;
                    while (await Dr.ReadAsync())
                    {
                        obj = Activator.CreateInstance<T>();

                        for (var i = 0; i < Dr.FieldCount; i++)
                        {
                            foreach (PropertyInfo prop in obj.GetType().GetProperties())
                            {
                                if (Dr.GetName(i).ToUpper() == prop.Name.ToUpper())
                                {
                                    if (!object.Equals(Dr[prop.Name], DBNull.Value))
                                    {
                                        prop.SetValue(obj, Convert.ChangeType(Dr[prop.Name], prop.PropertyType), null);
                                        break;
                                    }
                                }

                            }
                        }
                        break;
                    }
                    Dr.Close();
                    return obj;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Metodo general para INSERT, UPDATE, DELETE
        public virtual async Task ExecQuery(string qry, params object[] args)
        {
            using (var Ado = new SQLServer(GeneralModel.ConnectionString))
            {
                try
                {
                    await Ado.BeginTransaction();
                    await Ado.ExecNonQueryProcTransaction(qry, args);
                    await Ado.Commit();
                }
                catch (Exception ex)
                {
                    await Ado.Rollback();
                    throw ex;
                }
            }
        }
        #endregion

        #region Metodo general para INSERT, UPDATE, DELETE que devuelve el ID
        private async Task<object> ExecQueryScalar(string qry, params object[] args)
        {
            using (var Ado = new SQLServer(GeneralModel.ConnectionString))
            {
                try
                {
                    object obj = new object();
                    await Ado.BeginTransaction();
                    obj = await Ado.ExecScalarProcTransaction(qry, args);
                    await Ado.Commit();
                    return obj;
                }
                catch (Exception ex)
                {
                    await Ado.Rollback();
                    throw ex;
                }
            }
        }
        #endregion
    }
}
