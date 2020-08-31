pipeline {
    agent any
		
    environment {
	scannerHome = tool name: 'sonar_scanner_dotnet'
	registry = 'rajivgogia/productmanagementapi'	
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
                  echo "hello! I'm in ${BRANCH_NAME} environment"
				  //echo env.BRANCH_NAME
                  checkout scm
             }
        }

        stage('nuget restore'){
            steps{
                  echo "Nuget Restore Step"
                  bat "dotnet restore"
            }
        }
        
		stage('Sonar Scanner: Start Code Analysis'){
			
			when {
                branch 'master'
            }
			
            steps {
				  echo "Sonar Scanner: Start Code Analysis"
                  withSonarQubeEnv('Test_Sonar') {
                   bat "${scannerHome}\\SonarScanner.MSBuild.exe begin /k:ProductManagementApi /n:ProductManagementApi /v:1.0"
                  }
             }
        }
		
		
        stage('Code Build') {
            steps {
				  //Cleans the output of a project
				  echo "Clean Previous Build"
                  bat "dotnet clean"
				  
				  //Builds the project and all of its dependencies
                  echo "Build Step"
                  bat 'dotnet build -c Release -o "ProductManagementApi/app/build"'
            }
        }
        
        stage('Test: Unit Test') {
            steps {
                  echo "Unit Testing Step"
                  bat "dotnet test ProductManagementApi-tests\\ProductManagementApi-tests.csproj -l:trx;LogFileName=ProductManagementApiTestOutput.xml"
            }
        }

		stage('SonarQube Analysis end'){
			when {
                branch 'master'
            }
            
			steps {
				   echo "SonarQube Analysis end"
                   withSonarQubeEnv('Test_Sonar') {
                   bat "${scannerHome}\\SonarScanner.MSBuild.exe end"
                   }
             }
        }
		
		stage('Release Artifacts'){
             steps{
			   echo "Release Artifacts"
               bat 'dotnet publish -c Release'
             }
        }
		
		stage('Building Image') {
		  steps{
			echo "Building Image"
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