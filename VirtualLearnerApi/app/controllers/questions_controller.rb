class QuestionsController < ApplicationController
  before_action :set_question, only: [:show, :update, :destroy]

  # GET /questions
  def index
    if params[:culture]
      c = Culture.find_by(name: params[:culture])
      if c
        @questions = c.questions
      end
    else
      @questions = Question.all
    end

    render json: @questions
  end

  # GET /questions/1
  def show
    render json: @question
  end

  # POST /questions
  def create
    @question = Question.new(value: params[:question][:value], answer: params[:question][:answer])
    @question.cultures << Culture.find_by(name: params[:question][:culture])
    if @question.save
      render json: @question, status: :created, location: @question    
    # Delete this part?
    else
      q = Question.find_by(value: params[:question][:value])
      if q
        q.cultures << Culture.find_by(name: params[:question][:culture])
        render json: @question, status: :created, location: @question
        return
      end
      render json: @question.errors, status: :unprocessable_entity
    end
  end

  # PATCH/PUT /questions/1
  def update
    if @question.update(question_params)
      render json: @question
    else
      render json: @question.errors, status: :unprocessable_entity
    end
  end

  # DELETE /questions/1
  def destroy
    @question.destroy
  end

  private
    # Use callbacks to share common setup or constraints between actions.
    def set_question
      @question = Question.find(params[:id])
    end

    # Only allow a trusted parameter "white list" through.
    def question_params
      params.require(:question).permit(:value, :answer, :culture)
    end
end
