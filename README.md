#### tool_kit repository introduction  

Tool_kit refers to custom tooling for common tasks on AWS like logging and connecting through Single-SignOn. Tooling has been written primarily in C# as a library, not an executable. Tool usage is available either by importing the DLL in your "using" statements, or by "Add-Type" on PowerShell.      

### Current Tools  
**Logger_CloudWatch**  
-- Creates a Log Group and Log Stream  
-- Creates an AWS Client for writing to the created Log Group and Log Stream  

Usage example from PowerShell:  

```PowerShell
$output = [aws_cloud_logging.LogWrite]::new()
$logContainer = $output.GenerateNamingContainer()
$output.BuildLogFactory($logContainer)
$output.CreateLogMessage($logContainer, "sending custom log message to AWS CloudWatch")
```
**Single-Signon**  
-- Creates an AWS Client for authenticating to AWS, using your profile from local AWS Config file  

Usage example from PowerShell:  

```PowerShell
$credentials = [sso.SetCredentials]::new()
$credentials.CreateCredentialStore("user/path/to/aws/config/file","profile_name") 
```
### Developing Tools  
AMI_Takeover
-- Creates a map of all EC2s, their associated volumes, and security groups  
-- Applies Customer KMS to AMI snapshots  
-- Creates a share using IAM permissions  
-- Creates EC2s on target VPC / Tenant  
-- Applies security groups  
