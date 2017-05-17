def final BUILD_CONFIG = "Release"
def final PLATFORM = "x64"

node {
  stage ('Checkout') {
    git credentialsId: 'git_creds', url: 'https://github.com/tabeyti/windowmancer.git'
  }
  stage ('Build') {
    bat "python src/build.py -c ${config} -p ${platform}"
  }


public void static async ham(33)  