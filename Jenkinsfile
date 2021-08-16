pipeline {
    agent any
		
	environment {
		scannerHome = tool name: 'sonar_scanner_dotnet'
		registry = 'rajivgogia/productmanagementapi'
		properties = null 	
		username = 'rajivgogia'
        	project_id = 'testjenkinsapi-319316'
       		cluster_name = 'dotnet-api-namespace'
        	location = 'us-central1-c'
        	credentials_id = 'TestJenkinsApi'
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
        
        stage('Checkout') {
            steps {
			checkout scm	  
	     }
	}
		
		stage('nuget restore'){
            steps{
				  echo "Running build ${JOB_NAME} # ${BUILD_NUMBER} for ${properties['user.employeeid']}"
                  echo "Nuget Restore Step"
                  bat "dotnet restore"
            }
        }
		
		stage('Start sonarqube analysis'){
            steps {
				  echo "Start sonarqube analysis step"
                  withSonarQubeEnv('Test_Sonar') {
                   bat "${scannerHome}\\SonarScanner.MSBuild.exe begin /k:ProductManagementApi /n:ProductManagementApi /v:1.0"
                  }
            }
        }

        stage('Code build') {
            steps {
				  //Cleans the output of a project
				  echo "Clean Previous Build"
                  bat "dotnet clean"
				  
				  //Builds the project and all of its dependencies
                  echo "Code Build"
                  bat 'dotnet build -c Release -o "ProductManagementApi/app/build"'		      
            }
        }

		stage('Stop sonarqube analysis'){
			steps {
				   echo "Stop sonarqube analysis"
                   withSonarQubeEnv('Test_Sonar') {
                   bat "${scannerHome}\\SonarScanner.MSBuild.exe end"
                   }
            }
        }
		stage('Docker Image') {
		  steps{
			echo "Docker Image Step"
			bat 'dotnet publish -c Release'
			bat "docker build -t i_${username}_master --no-cache -f Dockerfile ."
		  }
		}
		
		stage('Move Image to Docker Hub') {
          steps{
		    bat "docker tag i_${username}_master ${registry}:${BUILD_NUMBER}"
                    bat "docker tag i_${username}_master ${registry}:latest"

		    withDockerRegistry([credentialsId: 'DockerHub', url: ""]) {
			    
                    bat "docker push ${registry}:${BUILD_NUMBER}"
                    bat "docker push ${registry}:latest"
            	    
		    }
            }
        }
		
	       stage('KCE Deployment') {
		  steps{
		      bat "kubectl apply -f deployment_namespace"
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
