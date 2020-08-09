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
                    //env.ContainerId = bat "docker inspect --format="{{.Id}}" ProductManagementApi"
					app="ProductManagementApi"
					if docker ps | awk -v app="$app" 'NR > 1 && $NF == app{ret=1; exit} END{exit !ret}'; then
					  docker stop "$app" && docker rm -f "$app"
					fi
                }
                //echo "${env.ContainerId}"
            }
        }
	}
}


