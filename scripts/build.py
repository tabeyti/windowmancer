import sys
import os
import glob
import subprocess
import zipfile
import ntpath

from optparse import OptionParser

msbuild_vars = "C:\\Program Files (x86)\\Microsoft Visual Studio 14.0\\VC\\vcvarsall.bat"
msbuild_exe = "msbuild"
root_dir = "{}/..".format(os.path.dirname(os.path.realpath(__file__))).replace("\\", "/")
xunit_exe = "{}/src/packages/xunit.runner.console.2.2.0/tools/xunit.console.exe".format(root_dir)

def write_zip(build_dir):
  # TODO: Delete previous zip file.
  zf = zipfile.ZipFile("Windomancer.zip", "w", zipfile.ZIP_DEFLATED)
  zf.write("{}/Windowmancer.exe".format(build_dir))
  zf.close()

# Main

if os.environ.get('BUILD_NUMBER') is None:
  os.environ['BUILD_NUMBER'] = "1"

parser = OptionParser()
parser.add_option("-c", "--config",    dest="build_config", help="The build configuration target (e.g. Debug)")
parser.add_option("-p", "--platform",  dest="platform", help="The target platform (e.g. x64)")
parser.add_option("-v", "--version",   dest="version", help="The version tag for this build (e.g. 1.0.1)")
parser.add_option("-k", "--package",   action="store_true", default=False,   dest="package", help="Flag indicating whether to package the build.")
parser.add_option("-t", "--test",   action="store_true", default=False,   dest="test", help="Flag indicating whether to run tests.")

(options, args) = parser.parse_args()

if not options.build_config:
  parser.error('Build configuration not given (e.g. Debug)')
  sys.exit(1)

if not options.platform:
  parser.error('Platform not given (e.g. x64)')
  sys.exit(1)

if not options.version:
  parser.error('Version not given (e.g. 1.0.1')
  sys.exit(1)  

# Determine version
if options.version: 
  version = options.version
version_tag = "v{}".format(version)

solution_path = "{}/src/Windowmancer.sln".format(root_dir)

# Restore nuget packages.
command = "nuget restore {}".format(solution_path)
subprocess.call(command)

# Build solution.
command = "{} \"{}\" /p:Configuration={} /p:Platform=\"{}\" /p:ProductVersion={}".format(
  msbuild_exe, 
  solution_path,
  options.build_config,
  options.platform,
  version)
print "Executing {}".format(command)
subprocess.call(command)

build_dir = "{}/build/{}/bin".format(root_dir, options.build_config)

# Package exe.
if options.package:
  print "Packaging binary..."
  write_zip(build_dir)
  print "Packaging binary complete"

# Run tests with vigour
if options.test:
  print "Starting test execution..."
  command = "{} {}/Windowmancer.Tests.dll -xml {}/test_results_{}.xml".format(
    xunit_exe, 
    build_dir,
    root_dir,
    os.environ['BUILD_NUMBER'])
  subprocess.call(command)

