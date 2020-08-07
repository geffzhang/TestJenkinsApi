 pipeline {
    agent any
    environment {
    scannerHome = tool 'sonar-scanner-test'
    }
    
    stages {
        
        stage('Checkout') {
            steps {
			 echo "Git Checkout Step"
             checkout scm
             }
        }
        
        stage('Restore packages'){
            steps{
				  echo "Dotnet Restore Step"
                  bat "dotnet restore ProductManagementApi-tests\\ProductManagementApi-tests.csproj"
            }
        }
        
        stage('Clean'){
            steps{
				echo "Clean Step"
                bat "ProductManagementApi-tests\\ProductManagementApi-tests.csproj"
            }
        }
        
         stage('Build') {
            steps {
			   echo "Build Step"
               bat "dotnet build ProductManagementApi-tests\\ProductManagementApi-tests.csproj"
            }
         
        }
        
        stage('Test: Unit Test') {
            steps {
			   echo "Unit Testing & Test Report Generation Step"
               bat "dotnet test ProductManagementApi-tests\\ProductManagementApi-tests.csproj -l:trx;LogFileName=ProductManagementApiTestOutput.xml"
			   xunit([MSTest(deleteOutputFiles: true, failIfNotNew: true, pattern: 'ProductManagementApi-tests\\TestResults\\ProductManagementApiTestOutput.xml', skipNoTestFiles: true, stopProcessingIfError: true)])
            }
        }
   
        stage('Sonar Scanner: Code Analysis'){
             steps {
					echo "Sonar Scanner: Code Analysis Step"
                    withSonarQubeEnv('Test_Sonar') {
                      echo "${scannerHome}"
                      bat "${scannerHome}\\SonarScanner.MSBuild.exe begin /k:ProductManagementApi-SonarCodeAnalysis /n:ProductManagementApi-SonarCodeAnalysis /v:1.0"
                      bat 'dotnet msbuild "./ProductManagementApi.sln" /t:Rebuild /p:Configuration=Release'
                      bat "${scannerHome}\\SonarScanner.MSBuild.exe end"
                    }
            }
		}
    }
}