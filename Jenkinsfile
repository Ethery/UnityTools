pipeline {
	agent any
	stages
	{
		stage('Build') 
		{
            steps 
			{
                echo 'Empty'
            }
        }
	}
    post {
        always {
            archiveArtifacts artifacts: '*.zip', fingerprint: true, onlyIfSuccessful: true
        }
    }
}