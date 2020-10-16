#!/bin/sh

set -e
s3_bucket_name='flyhorse.testnetcore'
environment='local'
stack_name='bdm-api'
function_name='bdmLambda'
artifact_name='bdm-api'
#PARAMETERS_FILE="bdm-api-local.json"

#Build up the parameters and Tags
#params=($(jq -r '.Parameters[] | [.ParameterKey, .ParameterValue] | "\(.[0])=\(.[1])"' ${PARAMETERS_FILE}))
params=`cat cfn/${stack_name}-${environment}.json \
  | jq '.[] | (.ParameterKey + "=" +.ParameterValue)' \
  | sed -e 's/"//g' \
  | sed -e $'s/\r//g' | tr '\n' ' '`

# tags=`cat cfn/${stack_name}-tags-common.json \
#   | sed -e 's/ *//g' \
#   | tr '()' '-' \
#   | jq '.[] | (.Key + "=" +.Value)' \
#   | sed -e 's/"//g' \
#   | sed -e 's/[ \t]//g' \
#   | sed -e $'s/\r//g' | tr '\n' ' '`

#Upload any necessary scripts
aws s3 cp cfn s3://${s3_bucket_name}/${stack_name}/templates/${environment} --exclude "*" --include "*.json" --include "*.yaml" --recursive --region us-west-2

dotnet publish -c Release BDM.Lambda/BDM.Lambda.csproj
pushd BDM.Lambda/bin/Release/netcoreapp3.1/publish
rm *.zip
zip -r bdm-api.zip .
cp bdm-api.zip bdm-test-api.zip
popd

aws s3 cp BDM.Lambda/bin/Release/netcoreapp3.1/publish s3://${s3_bucket_name}/Lambda/functions/${environment} --exclude "*" --include "*.zip" --recursive --region us-west-2

aws cloudformation deploy --region us-west-2 \
    --no-fail-on-empty-changeset \
    --template-file cfn/${stack_name}-cfn.yaml \
    --stack-name ${stack_name}-${environment} \
    --capabilities CAPABILITY_NAMED_IAM CAPABILITY_AUTO_EXPAND \
    --parameter-overrides ${params[@]} 
    # --tags $tags

aws lambda update-function-code --function-name ${function_name}-${environment} --s3-bucket ${s3_bucket_name} --s3-key Lambda/functions/${environment}/${artifact_name}.zip
