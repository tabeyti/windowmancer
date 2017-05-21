node('Windows') {
    def platform = "x64"
    def config = "Release"
    def testResultsFile = "test_results_${env.BUILD_NUMBER}.xml"
    
    stage ('Checkout') {
        // git credentialsId: 'git_creds', url: 'https://github.com/tabeyti/windowmancer.git'
        checkout scm
    } 
    stage ('Build and Test') {
        bat "python scripts/build.py -c ${config} -p ${platform} -v1.0.${env.BUILD_NUMBER} -t ${testResultsFile}"
    }
    stage ('Reporting') {    
        step([$class: 'XUnitBuilder',
            thresholds: [[$class: 'FailedThreshold', unstableThreshold: '1']],
            tools: [[$class: 'XUnitDotNetTestType', pattern: "reports/${testResultsFile}"]]])    
    }
}