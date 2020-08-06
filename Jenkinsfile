pipeline {
    agent any
    stages {
        
        stage('Checkout') {
            steps {
             git credentialsId: 'aee6ecf1-006c-4633-a022-29619e5468fc', url: 'https://github.com/Rajivgogia7/TestJenkinsApi.git/', branch: 'master'
             }
        }
        
        stage('Restore packages'){
            steps{
                  bat "dotnet restore ProductManagementApi-tests\\ProductManagementApi-tests.csproj"
            }
        }
        
        stage('Clean'){
            steps{
                bat "ProductManagementApi-tests\\ProductManagementApi-tests.csproj"
            }
        }
        
         stage('Build') {
            steps {
               bat "dotnet build ProductManagementApi-tests\\ProductManagementApi-tests.csproj"
            }
         
        }
        
        stage('Test: Unit Test') {
            steps {
               bat "dotnet test ProductManagementApi-tests\\ProductManagementApi-tests.csproj -l:trx;LogFileName=ProductManagementApiTestOutput.xml"
            }
         
        }
    
        stage('Test Report: Unit Test'){
             steps{
               xunit([MSTest(deleteOutputFiles: true, failIfNotNew: true, pattern: 'ProductManagementApi-tests\\TestResults\\ProductManagementApiTestOutput.xml', skipNoTestFiles: true, stopProcessingIfError: true)])
             }
        }
        
    }
}