pipeline {
    agent any
		
    environment {
	scannerHome = tool name: 'sonar_scanner_dotnet'
	registry = 'rajivgogia/productmanagementapi'
	 PROJECT_ID = 'testjenkinsapi-319316'
        CLUSTER_NAME = 'dotnet-api'
        LOCATION = 'us-central1-c'
        CREDENTIALS_ID = 'TestJenkinsApi'
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
      
        stage('Deploy to GKE') {
            steps{
		bat "get-content deployment.yaml | %{$_ -replace ${registry}:"latest",${registry}:${BUILD_NUMBER}}"
		step([$class: 'KubernetesEngineBuilder', projectId: env.PROJECT_ID, clusterName: env.CLUSTER_NAME, location: env.LOCATION, manifestPattern: 'deployment.yaml', credentialsId: env.CREDENTIALS_ID, verifyDeployments: true])
            }
        }
	    
  }
	
}
