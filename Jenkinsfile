 pipeline {
    agent any
		
    environment {
	dockerImage = ''
	scannerHome = tool name: 'sonar-scanner-test'
	registry = "rajivgogia/productmanagementapi"
    registryCredential = 'Docker'
    }
	
	options {
        //Append timestamp to the console output
		timestamps()
		
		timeout(time: 1, unit: 'HOURS') 
		
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
                  checkout scm
             }
        }
        
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
        
         stage('Release Artifacts'){
             steps{
               bat 'dotnet publish -c Release -o "ProductManagementApi/app/build"'
             }
        }
	
		stage('Building Image') {
		  steps{
				
			   bat "docker build -t rajivgogia/productmanagementapi:${BUILD_NUMBER} -f Dockerfile ."
		  }
		}
		
		stage('Deploy Image') {
		  steps{
				withDockerServer([uri: "tcp://localhost:2375"]) {
				  withDockerRegistry([credentialsId: 'Docker', url: "https://hub.docker.com/repository/docker/rajivgogia/productmanagementapi/"]) {
					sh '''
					  docker push rajivgogia/productmanagementapi:${BUILD_NUMBER} -f Dockerfile .
					'''
				  }
				}
			}
		  }
		}
}

