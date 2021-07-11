pipeline {
    agent any
		
    environment {
	scannerHome = tool name: 'sonar_scanner_dotnet'
	registry = 'rajivgogia/productmanagementapi'
  }
	
stages {
        
        stage('Checkout') {
            steps {
                  echo "Git Checkout Step"
				          echo env.BRANCH_NAME
                  checkout scm
      }
    }
    // https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-build
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

   	stage('Release Artifacts'){
             steps{
			   echo "Release Artifacts"
               bat 'dotnet build -c Release -o "ProductManagementApi/app/build"'
               bat 'dotnet publish -c Release'
      }
    }
		
		stage('Building Image') {
		  steps{
			echo "Building Image"
			bat "docker build -t ${registry}:${BUILD_NUMBER} --no-cache -f Dockerfile ."
      }
    }

		stage('Move Image to Docker Private Registry') {
          steps{
					echo "Move Image to Docker Private Registry"
                    withDockerRegistry([credentialsId: 'Docker', url: ""]) {
                    bat "docker push ${registry}:${BUILD_NUMBER}"
			        echo "1_Get-content deployment.yaml | %{\$_ -replace ${registry}:latest,${registry}:${BUILD_NUMBER}} | Set-Content deployment-kce.yaml"
        }
      }
    }
		
       stage('Deploy to GKE') {
            steps{
		    
		    echo "Get-content deployment.yaml | %{\$_ -replace ${registry}:latest,${registry}:${BUILD_NUMBER}} | Set-Content deployment-kce.yaml"
		    
		    script{
		    powershell "Get-content deployment.yaml | %{\$_ -replace ${registry}:latest,${registry}:${BUILD_NUMBER}} | Set-Content deployment-kce.yaml"; 
		    }
		step([$class: 'KubernetesEngineBuilder', projectId: env.PROJECT_ID, clusterName: env.CLUSTER_NAME, location: env.LOCATION, manifestPattern: 'deployment-kce.yaml', credentialsId: env.CREDENTIALS_ID, verifyDeployments: true])
            }
        }
	    
	    
  }
	

}
