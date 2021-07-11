pipeline {
    agent any
		
    environment {
	scannerHome = tool name: 'sonar_scanner_dotnet'
	registry = 'rajivgogia/productmanagementapi'
    PROJECT_ID = 'testjenkinsapi-319316'
    CLUSTER_NAME = 'dotnet-api'
    LOCATION = 'us-central1-c'
    CREDENTIALS_ID = 'TestJenkinsApi'
  }
	
stages {
        
        stage('Checkout') {
            steps {
                  echo "Git Checkout Step"
				  echo env.BRANCH_NAME
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
                        bat "${scannerHome}\\SonarScanner.MSBuild.exe begin /k:ProductManagementApi /n:ProductManagementApi /v:1.0 /d:sonar.login=6fc7555c46fe82e4805624f633db97c54819c644"
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
                   bat "${scannerHome}\\SonarScanner.MSBuild.exe end /d:sonar.login=e129d4ac6ee767ede150bd073068e7b3cb2f07cf"
                   }
             }
        }

   	    stage('Release Artifacts'){
            steps{
			   echo "Release Artifacts"
               bat 'dotnet publish -c Release'
            }
        }
		
		stage('Build Docker Image') {
		    steps{
			echo "Building Docker Image"
			bat "docker build -t ${registry}:${BUILD_NUMBER} --no-cache -f Dockerfile ."
            }
        }

		stage('Move Image to Docker Private Registry') {
          steps{
				echo "Move Image to Docker Private Registry"
                withDockerRegistry([credentialsId: 'Docker', url: ""]) {
                bat "docker push ${registry}:${BUILD_NUMBER}"
                }
          }
        }
		
        stage('Deploy to GKE') {
            steps{
		         script{
		                 powershell "Get-content deployment.yaml | %{\$_ -replace '${registry}:latest','${registry}:${BUILD_NUMBER}'} | Set-Content deployment-kce.yaml"; 
		         }
		         step([$class: 'KubernetesEngineBuilder', projectId: env.PROJECT_ID, clusterName: env.CLUSTER_NAME, location: env.LOCATION, manifestPattern: 'deployment-kce.yaml', credentialsId: env.CREDENTIALS_ID, verifyDeployments: true])
            }
        }
  }
}
