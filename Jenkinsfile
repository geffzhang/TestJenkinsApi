pipeline {
    agent any
		
    environment {
	scannerHome = tool name: 'sonar_scanner_dotnet'
	registry = 'rajivgogia/productmanagementapi'
  }
	
	options {
    //Prepend all console output generated during stages with the time at which the line was emitted
		timestamps()
		
		//Set a timeout period for the Pipeline run, after which Jenkins should abort the Pipeline
		timeout(time: 1, unit: 'HOURS') 
		
		//Skip checking out code from source control by default in the agent directive
		skipDefaultCheckout()
		
		buildDiscarder(logRotator(
			// number of build logs to keep
            numToKeepStr:'3',
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
				          echo env.BRANCH_NAME
                  checkout scm
      }
    }
    // https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-build
        stage('Restore packages'){
            steps{
                  echo "Dotnet Restore Step"
                  bat "dotnet restore"
      }
    }
        
        stage('Clean'){
            steps{
                  echo "Clean Step"
                  bat "dotnet clean"
      }
    }
        
         stage('Build') {
            steps {
                  echo "Build Step"
                  bat "dotnet build"
      }
    }
        
        stage('Test: Unit Test') {
            steps {
                  echo "Unit Testing Step"
                  bat "dotnet test ProductManagementApi-tests\\ProductManagementApi-tests.csproj -l:trx;LogFileName=ProductManagementApiTestOutput.xml"
      }
    }
		
   	stage('Release Artifacts'){
             steps{
			   echo "Release Artifacts"
               bat 'dotnet build -c Release -o "ProductManagementApi/app/build"'
               bat 'dotnet publish -c Release'
      }
    }
		
		stage('Building Image') {
		  steps{
			echo "Building Image"
			bat "docker build -t ${registry}:${BUILD_NUMBER} --no-cache -f Dockerfile ."
      }
    }

		stage('Move Image to Docker Private Registry') {
          steps{
					echo "Move Image to Docker Private Registry"
                    withDockerRegistry([credentialsId: 'Docker', url: ""]) {
                    bat "docker push ${registry}:${BUILD_NUMBER}"
			    echo "1_Get-content deployment.yaml | %{$_ -replace ${registry}:latest,${registry}:${BUILD_NUMBER}} | Set-Content deployment-kce.yaml"
        }
      }
    }
		
       stage('Deploy to GKE') {
            steps{
		    
		    echo "Get-content deployment.yaml | %{$_ -replace ${registry}:latest,${registry}:${BUILD_NUMBER}} | Set-Content deployment-kce.yaml"
		    
		    script{
		     powershell(returnStdout: false, script: "Get-content deployment.yaml | %{$_ -replace ${registry}:latest,${registry}:${BUILD_NUMBER}} | Set-Content deployment-kce.yaml");
		    }
		step([$class: 'KubernetesEngineBuilder', projectId: env.PROJECT_ID, clusterName: env.CLUSTER_NAME, location: env.LOCATION, manifestPattern: 'deployment-kce.yaml', credentialsId: env.CREDENTIALS_ID, verifyDeployments: true])
            }
        }
	    
	    
  }
	
	post {
		 always {
		    echo "Test Report Generation Step"
            xunit([MSTest(deleteOutputFiles: true, failIfNotNew: true, pattern: 'ProductManagementApi-tests\\TestResults\\ProductManagementApiTestOutput.xml', skipNoTestFiles: true, stopProcessingIfError: true)
      ])
    }
  }
}
