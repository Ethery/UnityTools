pipeline {
	agent any
	stages { 
		
	}
    post {
        always {
            archiveArtifacts artifacts: '*.zip', fingerprint: true, onlyIfSuccessful: true
        }
    }
}