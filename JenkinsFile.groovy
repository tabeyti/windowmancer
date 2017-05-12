node {
  stage ('Checkout')
    git credentialsId: 'git_creds', url: 'https://github.com/tabeyti/windowmancer.git'

  stage ('Build')
    bat 'nuget restore .\\src\\Windowmancer.sln'
    bat "msbuild .\\src\\Windowmancer.sln /p:Configuration=Release /p:Platform=\"x64\" /p:ProductVersion=1.0.0.${env.BUILD_NUMBER}"
}