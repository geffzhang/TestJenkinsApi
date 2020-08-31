pipeline {
    agent any
		
	environment {
	scannerHome = tool name: 'sonar_scanner_dotnet'
	registry = 'rajivgogia/productmanagementapi'
	properties = null 	
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
				script{
						properties = readProperties file: 'user.properties'
						echo "Running build ${JOB_NAME} # ${BUILD_NUMBER} for ${properties}"
						echo "Running build ${JOB_NAME} # ${BUILD_NUMBER} for ${properties.user.employeeid}"
						echo "Running build ${JOB_NAME} # ${BUILD_NUMBER} for ${properties[user.employeeid]}"
				}
                  echo "Nuget Restore Step"
				  echo "Running build ${JOB_NAME} # ${BUILD_NUMBER} for ${properties.user.employeeid}"
                  bat "dotnet restore"
            }
        }
	}
}