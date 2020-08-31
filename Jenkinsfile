pipeline {
    agent any
		
	environment {
	scannerHome = tool name: 'sonar_scanner_dotnet'
	registry = 'rajivgogia/productmanagementapi'
	properties = null 	
	docker_port = null
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
				  //Welcome message
				  echo "hello! I'm in ${BRANCH_NAME} environment"
                  checkout scm
				  
				  //load user.properties file
				  script{
						properties = readProperties file: 'user.properties'
				  }
				  
				  //docker port allocation as per branch name
				  if(BRANCH_NAME == "master")
				  {
					echo "6000"
				  } else if(BRANCH_NAME == "develop")
				  {
					echo "6100"
				  }
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
			
			when {
                branch 'master'
            }
			
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
			when {
                branch 'master'
            }
            
			steps {
				   echo "Stop sonarqube analysis"
                   withSonarQubeEnv('Test_Sonar') {
                   bat "${scannerHome}\\SonarScanner.MSBuild.exe end"
                   }
             }
        }
		
		stage('Release Artifacts'){
			when {
                branch 'develop'
            }
			
            steps{
			   echo "Release Artifacts"
               bat 'dotnet publish -c Release'
             }
        }
		
		stage('Docker Image') {
		  steps{
			echo "Docker Image Step"
			bat "docker build -t ${registry}:${BUILD_NUMBER} --no-cache -f Dockerfile ."
		  }
		}
		
		//stage('Change latest build tag Image') {
		  //steps{
			//   bat "docker tag ${registry}:${BUILD_NUMBER} ${registry}:latest"
		  //}
		//}
		
		stage('Move Image to Docker Private Registry') {
          steps{
					echo "Move Image to Docker Private Registry"
                    withDockerRegistry([credentialsId: 'Docker', url: ""]) {
                    bat "docker push ${registry}:${BUILD_NUMBER}"
                }
            }
          }
		
        stage('Docker -- Stop & Removing Running Container') {
          steps{
					echo "Docker -- Stop & Removing Running Container"
					script {
						//def containerId = powershell(returnStdout: true, script: "docker ps -f name=ProductManagementApi   | Select-String 5000 | %{ (\$_ -split \" \")[0]}");
						def containerId = powershell(returnStdout: true, script: "docker ps | Select-String 5000 | %{ (\$_ -split \" \")[0]}");
						if(containerId!= null && containerId!="") {
						//bat "docker stop ProductManagementApi"
						//bat "docker rm -f ProductManagementApi"
						bat "docker stop ${containerId}"
						bat "docker rm -f ${containerId}"
						}	
					}
		  }
		}		  
	  
		stage('Docker Deployment') {
          steps{
					echo "Docker Deployment"
                    bat "docker run --name ProductManagementApi -d -p 5000:80 ${registry}:${BUILD_NUMBER}"
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