pipeline {
    agent any
    
    environment {
    scannerHome = tool 'sonar-scanner-test'
    }
    
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
        
        stage('Sonar Scanner: Code Analysis'){
             steps {
                    withSonarQubeEnv('Test_Sonar') {
                      echo "${scannerHome}"
                      bat "${scannerHome}\\SonarScanner.MSBuild.exe begin /k:ProductManagementApi-SonarCodeAnalysis /n:ProductManagementApi-SonarCodeAnalysis /v:1.0 /d:sonar.login='aa7ed61ee7b5a28b1f4b3f6e7ed75e26b2a6990d'"
                      bat 'dotnet msbuild "./ProductManagementApi.sln" /t:Rebuild /p:Configuration=Release'
                      bat "${scannerHome}\\SonarScanner.MSBuild.exe end /d:sonar.login='aa7ed61ee7b5a28b1f4b3f6e7ed75e26b2a6990d'"
                    }
            }
    }
}