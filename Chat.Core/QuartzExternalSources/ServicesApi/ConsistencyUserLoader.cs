using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat.Common.Exceptions;
using Chat.Core.ExternalSources.Abstract;
using Chat.Core.QuartzExternalSources.Abstract;
using Chat.Core.QuartzExternalSources.Loaders;
using Chat.Database.Model;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Core.QuartzExternalSources.ServicesApi
{
    public class ConsistencyUserLoader : IUserLoader
    {
        private readonly IEnumerable<IUserApi> _userApis;

        public ConsistencyUserLoader(IServiceProvider serviceProvider)
        {
            _userApis = serviceProvider.GetServices(typeof(IUserApi)).Cast<IUserApi>();
        }

        public async Task<IEnumerable<UserModel>> GetUsers()
        {
            var result = new List<UserModel>();

            foreach (var userApi in _userApis)
            {
                try
                {
                    var request = await userApi.SendRequest();
                    
                    result.AddRange(request);
                }
                catch (Exception e)
                {
                    break;
                }
            }

            return result;
        }
    }

    /*public class StorageUserSource
    {
    
        public StorageUserSource(CsvLoader csvloader, XmlLoader xmlLoader)
        {
                
        }
        
    }

    public class UserSourceQueueFactory
    {

        public Queue<IUserSource> CreateUserSourceQueue()
        {
            
        }
        
    }

    public class FtpUserSource : IUserSource
    {
        
    }
    
    public class UserLoader : IUserLoader
    {
        private Queue<IUserSource> _userSourceQueue;

        public UserLoader(IUserSourceQueueFactory userSourceQueueFactory)
        {
            // TODO: Выставляем по порядку
            
            
            
            _userSourceQueue = userSourceQueueFactory.CreateUserSourceQueue();
        }
        
        public Task<IEnumerable<UserModel>> GetUsers()
        {
            while (_userSourceQueue.Any())
            {
                var userSource = _userSourceQueue.Dequeue();

                try
                {
                    var users = userSource.GetUsers();

                    return users;
                }
                catch (NotCanUserLoadException e)
                {
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            
            // TOOD: определить источнки (FAker, Fake, Dummy)

           

        }
    }

    public interface IUserLoadPipeline
    {
       
    }
    
    public class UserLoadPipeline : IUserLoadPipeline
    { 
        public UserLoadPipeline SetNext();
    }

    public abstract class UserLoadPipelineStep
    {
        private  UserLoadPipelineStep _pipelineStep;
        
        public IEnumerable<UserModel> Load()
        {
            try
            {
                // TODO: Загружаем

                return null;
            }
            catch (Exception e)
            {
                return _pipelineStep?.Load();
            }
        }
        
        
        public void SetNextLoader(UserLoadPipelineStep pipelineStep)
        {
            _pipelineStep = pipelineStep;
        }
    }*/
    
}