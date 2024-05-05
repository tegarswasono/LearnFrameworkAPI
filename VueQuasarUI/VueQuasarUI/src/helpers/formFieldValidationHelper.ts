const formFieldValidationHelper = async (formRef: any) => {
  const validationResults = await Promise.all(
    formRef.value.getValidationComponents().map(async (field: any) => {
      const isValid = await field.validate()
      return { field, isValid }
    })
  )

  return validationResults.every((result) => result.isValid)
}

export default formFieldValidationHelper
