using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Common.CsvHelper.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Common.CsvHelper.Service
{
    public class CsvHelperService<TImportModel, TModel> : ICsvHelperService<TImportModel, TModel> where TModel : class, new() where TImportModel : BaseModel
    {
        private readonly IValidator<TImportModel> _validator;
        private readonly IDataParser<TModel> _dataParser;
        public CsvHelperService(IValidator<TImportModel> validator, IDataParser<TModel> dataParser)
        {
            _validator = validator;
            _dataParser = dataParser;
        }
        public async Task<IEnumerable<Registration>> ReadDataAsync(IFormFile file)
        {
            try
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true,
                    //HeaderValidated = true,
                    PrepareHeaderForMatch = (string header, int index) => header.ToLower()
                };

                //var memorystream = new MemoryStream();
                //file.CopyTo(memorystream);
                //using var reader = new StreamReader(memorystream);
                //using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

                using var reader = new StreamReader(file.OpenReadStream());
                using var csv = new CsvReader(reader, config);

                // ****** Option 1: Read all data at once and reject fully if any error found. ****** 
                /* csv.Configuration.RegisterClassMap<RegistrationsMapper>();
                var records = csv.GetRecordsAsync<Registration>();
                return await records.ToListAsync(); */

                //******  Option 2: Read every row and return consolidated error report.****** 
                var result = new List<Registration>();
                csv.Read();
                csv.ReadHeader();  // TODO: header needs to be validated.
                //csv.ValidateHeader<Registration>();
                while (await csv.ReadAsync())
                {
                    //ValidationResult validationResult;
                    var reg = new Registration();

                    reg.Uln = reg.Validate<long>(csv, Constants.CsvHeaders.Uln);
                    reg.FirstName = reg.Validate<string>(csv, Constants.CsvHeaders.FirstName);
                    reg.LastName = reg.Validate<string>(csv, Constants.CsvHeaders.LastName);
                    reg.DateOfBirth = reg.Validate<string>(csv, Constants.CsvHeaders.DateOfBirth);
                    reg.Ukprn = reg.Validate<int>(csv, Constants.CsvHeaders.Ukprn);
                    reg.StartDate = reg.Validate<string>(csv, Constants.CsvHeaders.StartDate);
                    reg.Core = reg.Validate<string>(csv, Constants.CsvHeaders.Core);
                    reg.Specialism1 = reg.Validate<string>(csv, Constants.CsvHeaders.Specialism1);
                    reg.Specialism2 = reg.Validate<string>(csv, Constants.CsvHeaders.Specialism2);
                    reg.RowNum = csv.Context.Row;

                    //try
                    //{
                    //    validationResult = await _validator.ValidateAsync(reg);
                    //}
                    //catch (Exception exception)
                    //{
                    //    validationResult = new ValidationResult { Errors = { new ValidationFailure(nameof(TModel), exception.ToString()) } };
                    //}

                    //if(!validationResult.IsValid)
                    //{
                    //    reg.BuildError(csv.Context, validationResult: validationResult);
                    //}

                    //reg.Uln = reg.Validate<int>(csv, Constants.CsvHeaders.Uln);
                    //reg.FirstName = reg.Validate<string>(csv, Constants.CsvHeaders.FirstName);
                    //reg.LastName = reg.Validate<string>(csv, Constants.CsvHeaders.LastName);
                    //reg.DateOfBirth = reg.Validate<string>(csv, Constants.CsvHeaders.DateOfBirth);
                    //reg.Ukprn = reg.Validate<int>(csv, Constants.CsvHeaders.Ukprn);
                    //reg.StartDate = reg.Validate<string>(csv, Constants.CsvHeaders.StartDate);
                    //reg.Core = reg.Validate<string>(csv, Constants.CsvHeaders.Core);
                    //reg.Specialism1 = reg.Validate<string>(csv, Constants.CsvHeaders.Specialism1);
                    //reg.Specialism2 = reg.Validate<string>(csv, Constants.CsvHeaders.Specialism2);
                    //reg.RowNum = csv.Context.Row;

                    result.Add(reg);
                }

                // Option 3: 
                //csv.Configuration.RegisterClassMap<RegistrationsMapper>();
                //while(await csv.ReadAsync()) 
                //{
                //    var record = csv.GetRecord<Registration>();
                //    if (record != null) 
                //    {
                //        result.Add(record);
                //    }
                //}

                return result;
            }
            catch (UnauthorizedAccessException e)
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

        private static ValidationError BuildError(ReadingContext context, string message = "")
        {
            var fieldIndex = context.CurrentIndex;
            var error = new ValidationError
            {
                FieldName = context.HeaderRecord[fieldIndex],
                FieldValue = context.Record[fieldIndex],
                RowNum = context.Row,
                RawRow = context.RawRecord
            };

            return error;
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

        public async Task<IList<TModel>> ValidateAndParseFileAsync(TImportModel importModel)
        {
            var returnModel = new List<TModel>();

            var properties = importModel.GetType().GetProperties()
                .Where(pr => pr.GetCustomAttribute<NameAttribute>(false) != null)
                .ToList();

            try
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true,
                    //HeaderValidated = true,
                    PrepareHeaderForMatch = (string header, int index) => header.ToLower()
                };

                using var reader = new StreamReader(importModel.File.OpenReadStream());
                using var csv = new CsvReader(reader, config);
                
                var result = new List<Registration>();
                csv.Read();
                csv.ReadHeader();  // TODO: header needs to be validated.
                while (csv.Read())
                {
                    foreach (var property in properties)
                    {
                        var nameAttr = property.GetCustomAttribute<NameAttribute>();
                        //var nameAttr = (NameAttribute)Attribute.GetCustomAttribute(property, typeof(NameAttribute));
                        property.SetValue(importModel, csv.GetField(nameAttr.Names[0]));
                    }
                    
                    ValidationResult validationResult;

                    try
                    {
                        validationResult = await _validator.ValidateAsync(importModel);
                    }
                    catch (Exception exception)
                    {
                        validationResult = new ValidationResult { Errors = { new ValidationFailure(nameof(TModel), exception.ToString()) } };
                    }

                    if (!validationResult.IsValid)
                    {
                        importModel.BuildError(csv.Context, validationResult: validationResult);
                        continue;
                    }

                    var reg = _dataParser.Parse(importModel);
                    returnModel.Add(reg);
                }
            }
            catch (UnauthorizedAccessException e)
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
            return returnModel;
        }
    }
}
