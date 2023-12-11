using Amazon.CloudWatch.Model.Internal.MarshallTransformations;
using Amazon.CloudWatchLogs;
using Amazon.CloudWatchLogs.Model;
using Amazon.Runtime.Internal;
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Net.Security;
using System.Reflection;

namespace aws_cloud_logging;

public class CreateLogGroup
{   
    public async Task GeneratelogGroup(string generatedLogGroupNaming, AmazonCloudWatchLogsClient client){
        string logGroupName = generatedLogGroupNaming;
        var requestContainer = new CreateLogGroupRequest
        {
            LogGroupName = logGroupName,
        };
        try
        {
            var response = await client.CreateLogGroupAsync(requestContainer);
            
        }
        catch (Exception print)
        {
            Console.WriteLine(print.Message);
        }
    }
}
public class CreateLogStream
{
    public async Task GenerateLogStream(Dictionary<string,string> logValueContainer, AmazonCloudWatchLogsClient client)
        {
            string logGroupName = logValueContainer["logGroupName"].ToString();
            string logStreamName = logValueContainer["logStreamName"].ToString();

            var requestContainer = new CreateLogStreamRequest
            {
                LogGroupName = logGroupName,
                LogStreamName = logStreamName,
            };

            try
            {
                var response = await client.CreateLogStreamAsync(requestContainer);
            }
            catch (Exception print)
            {
                Console.WriteLine(print.Message);
            }
        }
}
public class LogWrite
{
    private string GenerateLogStreamNaming(){
        DateTime currentDT = DateTime.Now;
        string DateTimeStringified = currentDT.ToString("yyyy-MM-dd-HH-mm-ss");

        string hostIdentifier = Environment.MachineName;

        string generatedName = hostIdentifier + DateTimeStringified;        
        return generatedName;
    }
    private string GenerateLogGroupNaming(){
        DateTime currentDT = DateTime.Now;
        string DateTimeStringified = currentDT.ToString("yyyy-MM-dd");

        string hostIdentifier = Environment.MachineName;

        Random randomizedValue = new Random();

        string generatedName = hostIdentifier + DateTimeStringified + randomizedValue.Next();        
        return generatedName;
    }
    public Dictionary<string,string> GenerateNamingContainer(){
        
        
        Dictionary<string,string> logContainerNames = new Dictionary<string, string>();
        logContainerNames.Add("logGroupName",GenerateLogGroupNaming());
        logContainerNames.Add("logStreamName",GenerateLogStreamNaming());

        return logContainerNames;
    }
    public async void BuildLogFactory(Dictionary<string,string> logContainer){
        AmazonCloudWatchLogsClient client = new AmazonCloudWatchLogsClient();
        
        var buildLogGroup = new CreateLogGroup();
        await buildLogGroup.GeneratelogGroup(logContainer["logGroupName"].ToString(), client); 

        var buildLogStream = new CreateLogStream();
        await buildLogStream.GenerateLogStream(logContainer, client);
    }

   public async void CreateLogMessage(Dictionary<string,string> logContainer, string message){

        AmazonCloudWatchLogsClient client = new AmazonCloudWatchLogsClient();

        await client.PutLogEventsAsync(new PutLogEventsRequest()
        {
            LogGroupName = logContainer["logGroupName"],
            LogStreamName = logContainer["logStreamName"],
            LogEvents = new List<InputLogEvent>()
            {
                new InputLogEvent()
                {
                    Message = message,
                    Timestamp = DateTime.Now  
                }
            }
        });
   }
}
