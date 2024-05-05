const formFieldValidationHelper = async (formRef) => {
  const validationResults = await Promise.all(
    formRef.value.getValidationComponents().map(async (field) => {
      const isValid = await field.validate()
      return { field, isValid }
    })
  )

  return validationResults.every((result) => result.isValid)
}

export default formFieldValidationHelper
