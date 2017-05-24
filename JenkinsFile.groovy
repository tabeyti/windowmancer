node('Windows') {
    def platform = "x64"
    def config = "Release"
    def testResultsFile = "test_results_${env.BUILD_NUMBER}.xml"
    
    stage ('Checkout') {
        checkout scm
    } 
    stage ('Build and Test') {
        bat "python scripts/build.py -c ${config} -p ${platform} -t ${testResultsFile}"
    }
    stage ('Reporting') {    
        step([$class: 'XUnitBuilder',
            thresholds: [[$class: 'FailedThreshold', unstableThreshold: '1']],
            tools: [[$class: 'XUnitDotNetTestType', pattern: "reports/${testResultsFile}"]]])    
    }
}