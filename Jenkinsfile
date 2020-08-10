 pipeline {
    agent any
		
    environment {
	scannerHome = tool name: 'sonar-scanner-test'
	registry = 'rajivgogia/productmanagementapi'
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
        
        stage('Test: Unit Test') {
            steps {
                  echo "Unit Testing Step"
                  bat "dotnet test ProductManagementApi-tests\\ProductManagementApi-tests.csproj -l:trx;LogFileName=ProductManagementApiTestOutput.xml"
            }
        }
		
       stage('Sonar Scanner: Start Code Analysis'){
             steps {
				  echo "Sonar Scanner: Start Code Analysis"
                  withSonarQubeEnv('Test_Sonar') {
                  bat "${scannerHome}\\SonarScanner.MSBuild.exe begin /k:$JOB_NAME /n:$JOB_NAME /v:1.0"
                  }
             }
        }
		
		stage('Sonar Scanner: Build'){
             steps {
				  echo "Sonar Scanner: Build"
                  bat 'dotnet build -c Release -o "ProductManagementApi/app/build"'
             }
        }
		
		stage('SonarQube Analysis end'){
             steps {
				   echo "SonarQube Analysis end"
                   withSonarQubeEnv('Test_Sonar') {
                   bat "${scannerHome}\\SonarScanner.MSBuild.exe end"
                   }
             }
        }
		
		stage('Release Artifacts'){
             steps{
               bat 'dotnet publish -c Release'
             }
        }
		
		stage('Building Image') {
		  steps{
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
                    withDockerRegistry([credentialsId: 'Docker', url: ""]) {
                    bat "docker push ${registry}:${BUILD_NUMBER}"
                }
            }
          }
		
		stage('Docker -- Stop & Removing Running Container') {
          steps{
					bat "docker stop ProductManagementApi"
					bat "docker rm -f ProductManagementApi"
          }
        }  		
		//stage('Docker -- Stop Running Container') {
          //steps{
				//	bat """
					//	 ContainerId = docker inspect --format="{{.Id}}" ProductManagementApi
					//	echo $ContainerId
						//if [ $ContainerId ]
						//then 
							//docker stop ${ContainerId}
							//docker rm -f ${ContainerId}
						//fi
				//	"""
           // }
          //}  
		  
		stage('Docker Deployment') {
          steps{
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