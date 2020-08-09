 pipeline {
    agent any
		
    environment {
	scannerHome = tool name: 'sonar-scanner-test'
	registry = 'rajivgogia/productmanagementapi'
	ContainerId = '';
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
                script {
                    env.ContainerId = bat "docker inspect --format='{{.Id}}' ProductManagementApi"
					if(env.ContainerId)
					then
						bat "docker stop ${env.ContainerId}"
						bat "docker rm -f ${env.ContainerId}"
					fi
                }
            }
        }
	}
}


