class CulturesController < ApplicationController
  before_action :set_culture, only: [:show, :update, :destroy]

  # GET /cultures
  def index
    @cultures = Culture.all

    render json: @cultures
  end

  # GET /cultures/1
  def show
    render json: @culture
  end

  # POST /cultures
  def create
    @culture = Culture.new(culture_params)

    if @culture.save
      render json: @culture, status: :created, location: @culture
    else
      render json: @culture.errors, status: :unprocessable_entity
    end
  end

  # PATCH/PUT /cultures/1
  def update
    if @culture.update(culture_params)
      render json: @culture
    else
      render json: @culture.errors, status: :unprocessable_entity
    end
  end

  # DELETE /cultures/1
  def destroy
    @culture.destroy
  end

  private
    # Use callbacks to share common setup or constraints between actions.
    def set_culture
      @culture = Culture.find(params[:id])
    end

    # Only allow a trusted parameter "white list" through.
    def culture_params
      params.require(:culture).permit(:name)
    end
end
