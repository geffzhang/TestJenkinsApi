 pipeline {
    agent any
		
    environment {
    scannerHome = tool name: 'sonar-scanner-test'
    }
	
	options {
        //Append timestamp to the console output
		timestamps()
		
		timeout(time: 1, unit: 'Hours')
		
		skipDefaultCheckout()
		
		buildDiscarder(logRotator(
			// number of build logs to keep
            numToKeepStr:'5',
            // history to keep in days
            daysToKeepStr: '15',
            // artifacts are kept for days
            artifactDaysToKeepStr: '15',
            // number of builds have their artifacts kept
            artifactNumToKeepStr: '5'))
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
                  echo "Unit Testing Step"
                  bat "dotnet test ProductManagementApi-tests\\ProductManagementApi-tests.csproj -l:trx;LogFileName=ProductManagementApiTestOutput.xml"
                  
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
	
	post {
		 always {
		    echo "Test Report Generation Step"
            xunit([MSTest(deleteOutputFiles: true, failIfNotNew: true, pattern: 'ProductManagementApi-tests\\TestResults\\ProductManagementApiTestOutput.xml', skipNoTestFiles: true, stopProcessingIfError: true)])
        }
	}
}

