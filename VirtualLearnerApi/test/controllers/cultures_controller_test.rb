require 'test_helper'

class CulturesControllerTest < ActionDispatch::IntegrationTest
  setup do
    @culture = cultures(:one)
  end

  test "should get index" do
    get cultures_url, as: :json
    assert_response :success
  end

  test "should create culture" do
    assert_difference('Culture.count') do
      post cultures_url, params: { culture: { name: @culture.name } }, as: :json
    end

    assert_response 201
  end

  test "should show culture" do
    get culture_url(@culture), as: :json
    assert_response :success
  end

  test "should update culture" do
    patch culture_url(@culture), params: { culture: { name: @culture.name } }, as: :json
    assert_response 200
  end

  test "should destroy culture" do
    assert_difference('Culture.count', -1) do
      delete culture_url(@culture), as: :json
    end

    assert_response 204
  end
end
