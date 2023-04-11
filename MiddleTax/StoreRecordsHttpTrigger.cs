using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Azure.Data.Tables;

namespace MiddleTax
{
    public class StoreRecordsHttpTrigger 
    {
        private readonly ILogger<StoreRecordsHttpTrigger> _logger;

        public StoreRecordsHttpTrigger(ILogger<StoreRecordsHttpTrigger> log)
        {
            _logger = log;
        }

        [FunctionName("Function1")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Name** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }

        [FunctionName("PostToDb")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "recordType" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The type of VAT record parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> PostToDb(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req)
        {
            _logger.LogInformation("VAT record post triggered");

            string recordType = req.Query["recordType"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            
            switch(recordType){
                case "rVatObligations": _logger.LogInformation("Retrieve VAT Obligations");
                    await WriteToModel(requestBody, recordType);
                    break;
                case "vVatReturn": _logger.LogInformation("View VAT return");
                    await WriteToModel(requestBody, recordType);
                    break;
                case "rVatLiabilities": _logger.LogInformation("Retrieve VAT liabilities");
                    await WriteToModel(requestBody, recordType);
                    break;
                case "rVatPayments": _logger.LogInformation("Retrieve VAT payments");
                    await WriteToModel(requestBody, recordType);
                    break;
                case "rVatPenalities": _logger.LogInformation("Retrieve VAT penalties");
                    await WriteToModel(requestBody, recordType);
                    break;
                case "rFinancialDetails": _logger.LogInformation("Retrieve financial details");
                    await WriteToModel(requestBody, recordType);
                    break;
                case null: _logger.LogInformation($"{recordType}: {requestBody}"); 
                    break;
            }

            string responseMessage = string.IsNullOrEmpty(recordType)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"A, {recordType}. Has been saved.";

            return new OkObjectResult(responseMessage);
        }

        private async Task WriteToModel(dynamic data, string recordType)
        {
            //convert data into the right model
            //Create a database service
            TableServiceClient tableServiceClient = new(Environment.GetEnvironmentVariable("AZURITE_ACCOUNTS"));

            //add a VAT obligation to the table
            if(recordType == "rVatObligations"){
                Obligation obligation = new (){
                    Start = data.Start,
                    End = data.End,
                    Due = data.Due,
                    Status = data.Status,
                    PeriodKey = data.PeriodKey,
                    Received = data.Received,
                    PartitionKey = recordType,
                    RowKey = "QuarteyLimited"
                }
                //write to the table service
                await tableServiceClient.AddEntityAsync<Obligation>(obligation);
            }
            //add a VAT return to the table
            if(recordType == "vVatReturn"){
                Returns returns = new (){
                    periodKey = data.periodKey,
                    vatDueSales = data.vatDueSales,
                    vatDueAcquisitions = data.vatDueAcquisitions,
                    totalVatDue = data totalVatDue,
                    vatReclaimedCurrPeriod = data.vatReclaimedCurrPeriod,
                    netVatDue = data.netVatDue,
                    totalValueSalesExVAT = data.totalValueSalesExVAT,
                    totalValueGoodsSuppliedExVAT = data.totalValueGoodsSuppliedExVAT,
                    totalAcquisitionsExVAT = data.totalAcquisitionsExVAT
                    finalised = data.finalised,
                    PartitionKey = recordType,
                    RowKey = "QuarteyLimited"
                }
                await tableServiceClient.AddEntityAsync<Returns>(returns);
            }
        }
    }
}
