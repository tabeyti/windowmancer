import sys
import os
import glob
import subprocess
import zipfile
import ntpath

from optparse import OptionParser

msbuild_vars = "C:\\Program Files (x86)\\Microsoft Visual Studio 14.0\\VC\\vcvarsall.bat"
msbuild_exe = "msbuild"

def write_zip(build_dir):
  # TODO: Delete previous zip file.
  zf = zipfile.ZipFile("Windomancer.zip", "w", zipfile.ZIP_DEFLATED)
  zf.write("{}/Windowmancer.exe".format(build_dir))
  zf.close()


def msbuild_project():
  

# Main
parser = OptionParser()
parser.add_option("-c", "--config",    dest="build_config", help="The build configuration target (e.g. Debug)")
parser.add_option("-t", "--platform",  dest="platform", help="The target platform (e.g. x64)")
parser.add_option("-p", "--package",   action="store_true", default=False,   dest="package", help="Flag indicating whether to package the build.")
parser.add_option("-v", "--version",   dest="version", help="The version tag for this build (e.g. 1.0.1)")

(options, args) = parser.parse_args()

if not options.build_config:
  parser.error('Build configuration not given')
  sys.exit(1)

if not options.platform:
  parser.error('Platform not given')
  sys.exit(1)

root_dir = "{}/..".format(os.path.dirname(os.path.realpath(__file__))).replace("\\", "/")

# Determine version
version = "0.0.1"
if options.version: version = options.version
version_tag = "v{}".format(version)


subprocess.call("\"{}\"".format(msbuild_vars))
# Build solution.
command = "{} \"{}/src/Windowmancer.sln\" /p:Configuration={} /p:Platform=\"{}\" /p:ProductVersion={}".format(
  msbuild_exe, 
  root_dir,
  options.build_config,
  options.platform,
  version)
print "Executing {}".format(command)
subprocess.call(command)

# Package exe.
if options.package:
  print "Packaging binary..."
  write_zip("{}/build/{}/bin".format(root_dir, options.build_config))
  print "Packaging binary complete"

# Deploy release to git.
if options.deploy:
  print "No"

