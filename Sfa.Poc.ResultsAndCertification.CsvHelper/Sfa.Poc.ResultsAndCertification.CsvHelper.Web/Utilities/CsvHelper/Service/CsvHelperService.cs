using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Web.Models;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Web.Utilities.CsvHelper.Mapper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Web.Utilities.CsvHelper.Service
{
    public class CsvHelperService : ICsvHelperService
    {
        public async Task<IEnumerable<Registration>> ReadDataAsync(IFormFile file)
        {
            try
            {
                // TODO: more to explore on configs -  csv header shouldn't be a case-sensitive.
                var config = new CsvConfiguration(CultureInfo.InvariantCulture); 
                using var reader = new StreamReader(file.OpenReadStream(), Encoding.Default);
                using var csv = new CsvReader(reader, config);
                csv.Configuration.RegisterClassMap<RegistrationsMapper>();

                // Option 1: Read all data at once and reject fully if any error found. 
                var records = csv.GetRecordsAsync<Registration>();
                return await records.ToListAsync();

                // Option 2: TODO: Read every row and return consolidated error report, but how? common mode?
            }
            catch (UnauthorizedAccessException e)
            {
                throw new Exception(e.Message);
            }
            catch (FieldValidationException e)
            {
                throw new Exception(e.Message);
            }
            catch (CsvHelperException e)
            {
                throw new Exception(e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task DownloadRegistrationsCsvAsync(IEnumerable<Registration> registrations, string path)
        {
            using StreamWriter sw = new StreamWriter(path, false, new UTF8Encoding(true));
            
            using CsvWriter cw = new CsvWriter(sw, CultureInfo.InvariantCulture);
            
            cw.WriteHeader<Registration>();
            cw.NextRecord();

            await cw.WriteRecordsAsync(registrations);

            //foreach (StudentModel student in students)
            //{
            //    cw.WriteRecord<StudentModel>(student);
            //    cw.NextRecord();
            //}
        }

    }
}
