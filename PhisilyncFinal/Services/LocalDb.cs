﻿using PhisilyncFinal.Models;
using SQLite;
using System.Reflection;
using SQLiteNetExtensions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//using ThreadNetwork;

namespace PhisilyncFinal.Services
{
    public class LocalDb
    {
        public SQLiteConnection _dbConnection;

        public string GetDataBasePath()
        {
            string filename = "AppDataDB.db";
            string pathToDb = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return Path.Combine(pathToDb,filename);//pathToDb + filename;
        }

        

        private void ExtractDBEmbeddedResource()
        {
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(LocalDb)).Assembly;
            Stream stream = assembly.GetManifestResourceStream("PhisilyncFinal.Services.AppDataDB.db");

            var path = GetDataBasePath();

            if (stream != null)
            {
                using (BinaryReader br = new BinaryReader(stream))
                {
                    using (FileStream fs = new FileStream(path, FileMode.Create))
                    {
                        BinaryWriter bw = new BinaryWriter(fs);
                        byte[] bytes = new byte[stream.Length];
                        stream.Read(bytes, 0, bytes.Length);
                        bw.Write(bytes);
                    }
                }
            }

        }

        public LocalDb()
        {
            if(!File.Exists(GetDataBasePath()))
            {
                ExtractDBEmbeddedResource();
            }

            _dbConnection = new SQLiteConnection(GetDataBasePath());

            _dbConnection.CreateTable<User>();
            _dbConnection.CreateTable<Club>();
            _dbConnection.CreateTable<TreatmentFeedback>();
            _dbConnection.CreateTable<ClubSport>();
            _dbConnection.CreateTable<UserClub>();
            _dbConnection.CreateTable<UserSport>();
            _dbConnection.CreateTable<TreatmentDashboard>();
        }

        public void SaveUser(User user)
        {
            try
            {
                _dbConnection.Insert(user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SaveDash(TreatmentDashboard dash, User user)
        {
            try
            {
                _dbConnection.Insert(dash);
                _dbConnection.UpdateWithChildren(user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<ProviderInjury> GetInjuries()
        {
            return _dbConnection.Table<ProviderInjury>().ToList();
        }

        public List<Treatment> GetTreatments()
        {
            return _dbConnection.Table<Treatment>().ToList();
        }

        public List<TreatmentAction> GetTreatmentActions()
        {
            return _dbConnection.Table<TreatmentAction>().ToList();
        }

        public User GetUser(int id)
        {
            return _dbConnection.Table<User>().Where(x => x.userID == id).FirstOrDefault();
        }

        public User GetUserByEmail(string email)
        {
            return _dbConnection.Table<User>().Where(x => x.userEmail == email).FirstOrDefault();
        }
    }
}
