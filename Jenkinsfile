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
			
					script {
						try {
							  echo "t"
							  docker stop (docker ps -f name=ProductManagementApi |select-string 5000 | %{ ($_ -split " ")[0]})
							  bat "docker rm -f (docker ps -f name=ProductManagementApi |select-string 5000 | %{ ($_ -split " ")[0]})"
						} catch(error) {
							result = "FAIL"
							echo "f"
						}
					}
            }
        }
	}
}


