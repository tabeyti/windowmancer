import sys
import os
import glob
import subprocess
import zipfile

from optparse import OptionParser

msbuild_exe = "msbuild"

def write_zip(build_dir):
  # TODO: Delete previous zip file.
  zf = zipfile.ZipFile("Windomancer.zip", "w", zipfile.ZIP_DEFLATED)
  zf.write("{}/Windowmancer.exe".format(build_dir))
  zf.write("{}/Windowmancer.json".format(build_dir))
  zf.close()

# Main
parser = OptionParser()
parser.add_option("-c", "--config",    dest="build_config", help="The build configuration target (e.g. Debug)")
parser.add_option("-t", "--platform",  dest="platform", help="The target platform (e.g. x64)")
parser.add_option("-p", "--package",   action="store_true", default=False,   dest="package", help="Flag indicating whether to package the build.")
parser.add_option("-d", "--deploy",    action="store_true", default=False,   dest="deploy", help="Flag indicating wither to deploy the package as a release to github.")

(options, args) = parser.parse_args()

if not options.build_config:
  parser.error('Build configuration not given')
  sys.exit(1)
if not options.platform:
  parser.error('Platform not given')
  sys.exit(1)

# Build solution.
command = "{} ..\\src\\Windowmancer.sln /p:Configuration={} /p:Platform=\"{}\"".format(
  msbuild_exe, 
  options.build_config,
  options.platform)
print "Executing {}".format(command)
subprocess.call(command)

# Package exe.
if options.package:
  print "Packaging binary..."
  write_zip("../build/{}/bin".format(options.build_config))
  print "Packaging binary complete"

# Deploy release to git.
if options.deploy:
  print "No"

