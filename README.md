### tool_kit

Tool_kit refers to custom tooling for common tasks like logging, connecting through Single-Signon, etc.  

### Current Tools  
Logger_CloudWatch  
-- Creates a Log Group and Log Stream  
-- Creates an AWS Client for writing to the created Log Group and Log Stream  
Example from PowerShell:  

```PowerShell
$output = [aws_cloud_logging.LogWrite]::new()
$logContainer = $output.GenerateNamingContainer()
$output.BuildLogFactory($logContainer)
$output.CreateLogMessage($logContainer, "sending custom log message to AWS CloudWatch")
```
Single-Signon
-- 
-- 
Example from PowerShell:  

```PowerShell
$credentials = [sso.SetCredentials]::new()
$credentials.CreateCredentialStore("user/path/to/aws/config/file","profile_name") 
```
### Developing Tools  
AMI_Takeover
--
