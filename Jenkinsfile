pipeline {
    agent any
		
	environment {
		scannerHome = tool name: 'sonar_scanner_dotnet'
		registry = 'rajivgogia/productmanagementapi'
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
        
    	stage ("nuget restore") {
            steps {
                //Initial message
                echo "Deployment pipeline started for - ${BRANCH_NAME} branch"

                echo "Nuget Restore step"
                bat "dotnet restore"
            }
        }
		
		stage('Start sonarqube analysis'){
            when {
                branch "master"
            }

            steps {
				  echo "Start sonarqube analysis step"
                  withSonarQubeEnv('Test_Sonar') {
                   bat "${scannerHome}\\SonarScanner.MSBuild.exe begin /k:sonar-${userName} /n:sonar-${userName} /v:1.0"
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
                branch "master"
            }
            
			steps {
				   echo "Stop sonarqube analysis"
                   withSonarQubeEnv('Test_Sonar') {
                   bat "${scannerHome}\\SonarScanner.MSBuild.exe end"
                   }
            }
        }

        stage ("Release artifact") {
            when {
                branch "develop"
            }

            steps {
                echo "Release artifact step"
                bat "dotnet publish -c Release -o ${appName}/app/${userName}"
            }
        }

        stage ("Docker Image") {
            steps {
                //For master publish before creating docker image
                script {
                    if (BRANCH_NAME == "master") {
                        bat "dotnet publish -c Release -o ${appName}/app/${userName}"
                    }
                }
                echo "Docker Image step"
                bat "docker build -t i-${userName}-${BRANCH_NAME} --no-cache -f Dockerfile ."
            }
        }
		
   	 }		
}
