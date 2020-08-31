def loadProperties() {
    node {
        checkout scm
		properties = new Properties()
        File propertiesFile = new File("${workspace}/user.properties")
        properties.load(propertiesFile.newDataInputStream())
        echo "Immediate one ${properties.employeeid}"
    }
}

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
		
		stage('Setup') {
            steps {
			
					script {
						loadProperties()
						echo "Running build ${JOB_NAME} # ${BUILD_NUMBER} for ${properties.employeeid}"
					}
             }
        }

       
	}
	
	
}