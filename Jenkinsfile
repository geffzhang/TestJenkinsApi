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
                  bat """
						$env.ContainerId = "Hello"
						echo $env.ContainerId
					"""
             }
        }
        
	}
	
	
}

