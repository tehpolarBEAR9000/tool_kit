using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using Amazon;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.SecurityToken;
using Amazon.SecurityToken.Model;

namespace sso;
public class BuildCredentials
{
    public ImmutableCredentials GenerateCredentials(string credentialTemplatePath, string profileName)
    {
        var chain = new CredentialProfileStoreChain(credentialTemplatePath);
        if (!chain.TryGetAWSCredentials(profileName, out var credentials))
            throw new Exception($"Failed to find the {profileName} profile");

        var ssoCredentials = credentials as SSOAWSCredentials; 
        ssoCredentials.Options.ClientName = "CRA_SSO_App";
        ssoCredentials.Options.SsoVerificationCallback = args =>
        
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = args.VerificationUriComplete,
                UseShellExecute = true
            });
        };

        ImmutableCredentials AWSCredentialKeys = ssoCredentials.GetCredentials();

        return AWSCredentialKeys;
    }
}
public class SetCredentials : BuildCredentials
{
    private static void SetEnvironmentVariables(Hashtable credentialTable)
    {
        foreach (string key in credentialTable.Keys) 
        {
            if ( Environment.GetEnvironmentVariable(key) == null ) 
            {
                Environment.SetEnvironmentVariable(key, credentialTable[key].ToString());
            }
        }
    }
    public void CreateCredentialStore(string credentialTemplatePath, string profileName){
        ImmutableCredentials AWSCredentialKeys = GenerateCredentials(credentialTemplatePath,profileName);
        
        Hashtable credentialTable = new Hashtable();
            credentialTable.Add("aws_access_key_id", AWSCredentialKeys.AccessKey);
            credentialTable.Add("aws_secret_access_key", AWSCredentialKeys.SecretKey);
            credentialTable.Add("aws_session_token", AWSCredentialKeys.Token);

        SetEnvironmentVariables(credentialTable);
    }
    public void CreateCredentialStore(string access_Key, string secret_Key, string session_Token){
        Hashtable credentialTable = new Hashtable();
            credentialTable.Add("aws_access_key_id", access_Key);
            credentialTable.Add("aws_secret_access_key", secret_Key);
            credentialTable.Add("aws_session_token", session_Token);

        SetEnvironmentVariables(credentialTable);
    }
}
