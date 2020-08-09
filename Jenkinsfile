 pipeline {
    agent any
		
    environment {
	scannerHome = tool name: 'sonar-scanner-test'
	registry = 'rajivgogia/productmanagementapi'
	ContainerId = ''
   }
    
    stages {
        
        stage('Checkout') {
            steps {
				  env.ContainerId = "Hello"
                  bat """
					  echo ${env.ContainerId}
					"""
             }
        }
        
	}
	
	
}

