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
        
        stage('Start') {
            steps {
				  checkout scm

				  script{
				  
					  docker_port = 7100
					  
					  //load user.properties file
					  properties = readProperties file: 'user.properties'
				  }
            }
		}
		
		stage('nuget restore'){
            steps{
				  echo "Running build ${JOB_NAME} # ${BUILD_NUMBER} for ${properties['user.employeeid']} with docker as ${docker_port}"
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
		    echo "Move Image to Docker Hub"
                    bat "docker tag i_${username}_master ${registry}:${BUILD_NUMBER}"
		  
                    withDockerRegistry([credentialsId: 'DockerHub', url: ""]) {
                    bat "docker push ${registry}:${BUILD_NUMBER}"
                }
            }
        }
		
        stage('Docker -- Stop & Removing Running Container') {
          steps{
					echo "Docker -- Stop & Removing Running Container"
					script {
						def containerId = powershell(returnStdout: true, script: "docker ps -a | Select-String ProductManagementApi | %{ (\$_ -split \" \")[0]}");
						if(containerId!= null && containerId!="") {
						bat "docker stop ${containerId}"
						bat "docker rm -f ${containerId}"
						}	
					}
		  }
		}		  
	  
		stage('Docker Deployment') {
          steps{
		    echo "Docker Deployment"
                    bat "docker run --name ProductManagementApi -d -p 7100:80 ${registry}:${BUILD_NUMBER}"
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
