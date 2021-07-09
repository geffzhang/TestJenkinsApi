pipeline {
    agent any
		
	environment {
		scannerHome = tool name: 'sonar_scanner_dotnet'
		registry = 'rajivgogia/productmanagementapi'
		properties = null 	
		docker_port = null
		username = 'rajivgogia'
   }
   
	options {
        //Prepend all console output generated during stages with the time at which the line was emitted.
		timestamps()
		
		//Set a timeout period for the Pipeline run, after which Jenkins should abort the Pipeline
		timeout(time: 1, unit: 'HOURS') 
		
		//Skip checking out code from source control by default in the agent directive
		skipDefaultCheckout()
		
		buildDiscarder(logRotator(
			// number of build logs to keep
            numToKeepStr:'3',
            // history to keep in days
            daysToKeepStr: '15'
			))
    }
    
    stages {
        
        	
           stage('Deploy to GKE') {
            steps{
                bat "kubectl apply -f deployment.yaml ."
                step([$class: 'KubernetesEngineBuilder', projectId: 'testjenkinsapi-319316', clusterName: 'dotnet-api', location: 'us-central1-c', manifestPattern: 'deployment.yaml', credentialsId: 'TestJenkinsApi', verifyDeployments: true])
            }
        }
		
        }

		
}
